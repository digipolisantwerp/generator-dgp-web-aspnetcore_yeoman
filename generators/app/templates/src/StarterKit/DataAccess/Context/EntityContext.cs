using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using StarterKit.Entities;
using StarterKit.Utilities.Configs;

namespace StarterKit.DataAccess.Context
{
	public class EntityContext : DbContext
    {
        public EntityContext()
        { }

        public EntityContext(IDatabaseConfiguration dbConfig) : base(dbConfig.ConnectionString)
        {
            _dbConfig = dbConfig;
			this.Configuration.LazyLoadingEnabled = false;
        }

        private readonly IDatabaseConfiguration _dbConfig;

		// ToDo : voeg hier de entities toe
		//public DbSet<MyEntity> MyEntities { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			
			base.OnModelCreating(modelBuilder);

			// PostgreSQL gebruikt schema 'main' - niet dbo.
			modelBuilder.HasDefaultSchema("main");

			// Lowercase
			modelBuilder.Types().Configure(c =>
			{
				c.ToTable(c.ClrType.Name.ToLower());
			});

			modelBuilder.Properties().Configure(c =>
			{
				c.HasColumnName(c.ClrPropertyInfo.Name.ToLower());
			});

			// Cascading deletes afzetten voor de veiligheid
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

			// Pluralizing table names afzetten
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			ProcessHasPrecisionAttributes(modelBuilder);
		}

		private void ProcessHasPrecisionAttributes(DbModelBuilder modelBuilder)
		{
			var classTypes = ( from t in Assembly.GetAssembly(typeof(HasPrecisionAttribute)).GetTypes()
							   where t.IsClass && t.Namespace == "StarterKit.Entities"
							   select t ).ToList();



			foreach ( var classType in classTypes )
			{
				var propAttributes = classType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
												.Where(p => p.GetCustomAttribute<HasPrecisionAttribute>() != null)
												.Select(p => new { prop = p, attr = p.GetCustomAttribute<HasPrecisionAttribute>(true) });

				foreach ( var propAttr in propAttributes )
				{
					var entityConfig = modelBuilder.GetType().GetMethod("Entity").MakeGenericMethod(classType).Invoke(modelBuilder, null);
					ParameterExpression param = ParameterExpression.Parameter(classType, "c");
					Expression property = Expression.Property(param, propAttr.prop.Name);
					LambdaExpression lambdaExpression = Expression.Lambda(property, true, new ParameterExpression[] { param });
					DecimalPropertyConfiguration decimalConfig;
					if ( propAttr.prop.PropertyType.IsGenericType && propAttr.prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) )
					{
						MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[7];
						decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
					}
					else
					{
						MethodInfo methodInfo = entityConfig.GetType().GetMethods().Where(p => p.Name == "Property").ToList()[6];
						decimalConfig = methodInfo.Invoke(entityConfig, new[] { lambdaExpression }) as DecimalPropertyConfiguration;
					}

					decimalConfig.HasPrecision(propAttr.attr.Precision, propAttr.attr.Scale);
				}
			}
		}
	}
}

using System;
using System.Text;

namespace Digipolis.Helpers
{
    public class ExceptionHelper
    {
        /// <summary>
        /// Geeft de messages van alle inner exceptions.
        /// </summary>
        /// <param name="ex">De exception waarvan de messages moeten opgehaald worden.</param>
        /// <returns>Een string met de messages van alle inner exceptions.</returns>
        public static string GetAllMessages(Exception ex)
        {
            if ( ex == null ) return "null";
            var currentEx = ex;
            var builder = new StringBuilder();
            do
            {
                builder.AppendLine(currentEx.Message);
                currentEx = currentEx.InnerException;
            } while ( currentEx != null );
            return builder.ToString();
        }

        /// <summary>
        /// Geeft de stacktraces van alle inner exceptions.
        /// </summary>
        /// <param name="ex">De exception waarvan de stacktraces moeten opgehaald worden.</param>
        /// <returns>Een string met de stacktraces van alle inner exceptions.</returns>
        public static string GetAllStacktraces(Exception ex)
        {
            if ( ex == null ) return "null";
            var currentEx = ex;
            var builder = new StringBuilder();
            do
            {
                builder.AppendLine(currentEx.StackTrace);
                currentEx = currentEx.InnerException;
            } while ( currentEx != null );
            return builder.ToString();
        }

        /// <summary>
        /// Geeft de ToStrings() van alle inner exceptions. De ToString() method van een exception geeft zowel de Message als de Stacktrace.
        /// </summary>
        /// <param name="ex">De exception waarvan de messages en stacktraces moeten opgehaald worden.</param>
        /// <returns>Een string met de messages en stacktraces van alle inner exceptions.</returns>
        public static string GetAllToStrings(Exception ex)
        {
            if ( ex == null ) return "null";
            var currentEx = ex;
            var builder = new StringBuilder();
            do
            {
                builder.AppendLine(currentEx.ToString());
                currentEx = currentEx.InnerException;
            } while ( currentEx != null );
            return builder.ToString();
        }
    }
}
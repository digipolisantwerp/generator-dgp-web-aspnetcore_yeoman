/**
 * Import acpaas modules here
 * see: https://github.com/digipolisantwerp/acpaas-ui_angular for available components.
 */
import { TableModule } from "@acpaas-ui/ngx-table";
import { ItemCounterModule, PaginationModule } from "@acpaas-ui/ngx-pagination";
import { DatepickerModule } from "@acpaas-ui/ngx-forms";
import { FooterModule, HeaderModule } from "@acpaas-ui/ngx-layout";

export const AuiModules = [
  HeaderModule,
  FooterModule,
  TableModule,
  ItemCounterModule,
  PaginationModule,
  DatepickerModule,
];

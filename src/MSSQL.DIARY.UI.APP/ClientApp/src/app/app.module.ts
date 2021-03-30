import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { NgMarqueeModule } from 'ng-marquee';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { AmexioEnterpriseModule, AmexioWidgetModule, AmexioDataModule, AmexioChartsModule, AmexioLayoutModule } from 'amexio-ng-extensions';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { NgxUiLoaderModule } from 'ngx-ui-loader';

import { ProgressBarModule } from "angular-progress-bar" ; 
import { AccordionModule       } from 'primeng/accordion';
import { TableModule           } from 'primeng/table';
import { TreeModule            } from 'primeng/tree';
import { ToastModule           } from 'primeng/toast';
import { ButtonModule          } from 'primeng/button';
import { ContextMenuModule     } from 'primeng/contextmenu';
import { TabViewModule         } from 'primeng/tabview';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { BreadcrumbModule      } from 'primeng/breadcrumb';

 import{HighlightPipe} from 'src/pipe/HighlightPipe'
 import{SearchPipe} from 'src/pipe/SearchPipe'

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component'; 
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { LeftMenuComponent } from './left-menu/left-menu.component';
import { DatabaseServerComponent } from './database-server/database-server.component';
import { DatabaseComponent } from './databases/database.component';
import { DatabaseServerService } from '../services/database-server.service';
import { DatabaseService } from '../services/database.service';
import { DatabaseTableService } from '../services/database-table.service';
import { DatabaseTablesComponent } from './database-tables/database-tables.component';
import { DatabaseTableComponent } from './database-table/database-table.component';
import { DatabaseViewComponent } from './database-view/database-view.component';
import { DatabaseViewsComponent } from './database-views/database-views.component';
import { UploadComponent } from './upload/upload.component';



const appRoutes: Routes = [

  { path: '', component: HomeComponent, pathMatch: 'full' }, 
  { path: 'DatabaseServer', component: DatabaseServerComponent, canActivate: [AuthorizeGuard] },
  { path: 'Database/:databaseDetails', component: DatabaseComponent, canActivate: [AuthorizeGuard] },
  { path: 'DatabaseTables/:databaseDetails', component: DatabaseTablesComponent, pathMatch: 'full', canActivate: [AuthorizeGuard]},
  { path: 'DatabaseTable/:databaseDetails', component: DatabaseTableComponent, pathMatch: 'full', canActivate: [AuthorizeGuard]},
  { path: 'DatabaseViews/:databaseDetails', component: DatabaseViewsComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] },
  { path: 'DatabaseView/:databaseDetails', component: DatabaseViewComponent, pathMatch: 'full', canActivate: [AuthorizeGuard] },
 
  // {
  //   path: 'AllViews/:dbName',
  //   component: DatabaseAllViewsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'ViewsDetails/:dbName',
  //   component: DatabaseViewsDetailsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'StoreProcedures/:dbName',
  //   component: DatabaseAllStoreProcedureComponent,
  //   pathMatch: 'full'
  // }
  // ,
  // {
  //   path: 'StoreProcedureDetails/:dbName',
  //   component: DatabaseStoreProcedureDetailsComponent,
  //   pathMatch: 'full'
  // },

  // {
  //   path: 'DatabaseInfo/:dbName',
  //   component: DatabaseInfoComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'TableValueFunctions/:dbName',
  //   component: DatabaseTableValueFunctionsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'TableValueFunctionDetails/:dbName',
  //   component: DatabaseTableValueFunctionDetailsComponent,
  //   pathMatch: 'full'
  // }
  // ,
  // {
  //   path: 'ScalarFunctions/:dbName',
  //   component: DatabaseScalarFunctionsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'ScalarFunctionDetails/:dbName',
  //   component: DatabaseScalarFunctionDetailsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'DatabaseTriggers/:dbName',
  //   component: DatabaseTriggersComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'DatabaseTriggerDetails/:dbName',
  //   component: DatabaseTriggerDetailsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'UserDefinedDataTypes/:dbName',
  //   component: DatabaseUsedDefinedDataTypesComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'UserDefinedDataTypeDetails/:dbName',
  //   component: DatabaseUsedDefinedDataTypeDetailsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'XmlSchemaCollections/:dbName',
  //   component: DatabasexmlSchemaCollectionsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'XmlSchemaCollectionDetails/:dbName',
  //   component: DatabasexmlSchemaCollectionDetailsComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'WorkFlow/:dbName',
  //   component: BusinessworkFlowComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'LoadSSISPackage',
  //   component: LoadSSISPackagesComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'SSISPackage',
  //   component: SSISPackageComponent,
  //   pathMatch: 'full'
  // },

  // {
  //   path: 'SSISPackagesDetails/:PkgName',
  //   component: SSISPackageDetailsComponent,
  //   pathMatch: 'full'
  // },

  // {
  //   path: 'AllSchemas/:dbName',
  //   component: DatabaseSchemasComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'SchemasDetails/:dbName',
  //   component: DatabaseSchemaDetailsComponent,
  //   pathMatch: 'full'
  // },
  // //DatabaseBusinessModuleComponent

  // {
  //   path: 'AdminLogin',
  //   component: AdminloginComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: '',
  //   component: ServerInformationComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'BusinessModule/:dbName',
  //   component: DatabaseBusinessModuleComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'BusinessModuleCreate/:dbName',
  //   component: DatabaseBusinessModuleCreateComponent,
  //   pathMatch: 'full'
  // },
  // {
  //   path: 'BusinessModuleDetails/:dbName',
  //   component: DatabaseBusinessModuleDetailsComponent,
  //   pathMatch: 'full'
  // },
  //DatabaseBusinessModuleComponent
];

@NgModule({
  declarations: [
    SearchPipe,
    HighlightPipe,
    AppComponent,
    NavMenuComponent,
    HomeComponent, 
    LeftMenuComponent,
    DatabaseServerComponent,
    DatabaseComponent,
    DatabaseTablesComponent,
    DatabaseTableComponent,
    DatabaseViewComponent,
    DatabaseViewsComponent,
    UploadComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    //BrowserAnimationsModule,
    NoopAnimationsModule,
    HttpClientModule,
    FormsModule,
    NgMarqueeModule,
    ApiAuthorizationModule,
    RouterModule.forRoot(appRoutes),
    AngularSplitModule.forRoot(),
    AmexioDataModule,
    AmexioWidgetModule,
    AmexioChartsModule,
    AmexioEnterpriseModule,
    AmexioLayoutModule,
    FontAwesomeModule,
    NgxUiLoaderModule,
    BreadcrumbModule,
    ProgressBarModule,
    TreeModule,
    ToastModule,
    ButtonModule,
    ContextMenuModule,
    TabViewModule,
    CodeHighlighterModule       
    ,AccordionModule      
    ,TableModule          
    ,TreeModule           
    ,ToastModule          
    ,ButtonModule         
    ,ContextMenuModule    
    ,TabViewModule        
    ,CodeHighlighterModule
    ,BreadcrumbModule     
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    DatabaseServerService,
    DatabaseService,
    DatabaseTableService

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

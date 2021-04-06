import { Component, Inject, OnInit } from '@angular/core'; 
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DatabaseTableService } from 'src/services/database-table.service';
import { Observable } from 'rxjs';
import { TableIndexInfo } from 'src/models/TableIndexInfo';
import { TableCreateScript } from 'src/models/TableCreateScript';
import { Tabledependencies } from 'src/models/Tabledependencies';
import { TableColumns } from 'src/models/TableColumns';
import { Ms_Description } from 'src/models/Ms_Description';
import { TableFKDependency } from 'src/models/TableFKDependency';
import { TableKeyConstraint } from 'src/models/TableKeyConstraint';
import { TableFragmentationDetails } from 'src/models/TableFragmentationDetails';
import { MenuItem } from 'primeng/api';
import 'prismjs';
import 'prismjs/plugins/toolbar/prism-toolbar'; 
import 'prismjs/plugins/copy-to-clipboard/prism-copy-to-clipboard';
import 'prismjs/components/prism-css';
import 'prismjs/components/prism-javascript';
import 'prismjs/components/prism-java';
import 'prismjs/components/prism-markup';
import 'prismjs/components/prism-typescript';
import 'prismjs/components/prism-sass';
import 'prismjs/components/prism-scss';
declare var Prism: any;

@Component({
  selector: 'app-database-table',
  templateUrl: './database-table.component.html',
  styleUrls: ['./database-table.component.css']
})

export class DatabaseTableComponent implements OnInit {
  iblnShowEditBox: boolean;
  constructor(private route: Router,public activeRoute: ActivatedRoute, public databaseTableService: DatabaseTableService, public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {

    this.iblnShowTableCreateScript = false;
  }
  public items: MenuItem[];
  public home: MenuItem; 
  public language = 'plsql';
  public databaseName: string; 
  public databaseTableName: string;
  public tableDescription: string;
  public tableDescriptionOld: string;
  public databaseTableIndexs: Observable<TableIndexInfo[]>;
  public databaseTableCreateScript: TableCreateScript;
  public databaseTableDependencies: Observable<Tabledependencies[]>;
  public databaseTableColumns: TableColumns[];
  public databaseTableDescription: Ms_Description;
  public databaseTableFkDependencies: Observable<TableFKDependency[]>;
  public databaseTableKeyConstraints: Observable<TableKeyConstraint[]>;
  public databaseTableDependencyTree: any;
  public databaseTableFragmentationDetails: Observable<TableFragmentationDetails[]>;
  public iblnShowTableCreateScript:boolean;
 

  ngOnInit() {

    this.databaseName = this.activeRoute.snapshot.params.databaseDetails.split('/')[0]; 
    this.databaseTableName = this.activeRoute.snapshot.params.databaseDetails.split('/')[1];

    this.databaseTableDependencies = this.databaseTableService.LoadTableDependencies(this.databaseTableName, this.databaseName);
    this.databaseTableIndexs = this.databaseTableService.LoadTableIndexes(this.databaseTableName, this.databaseName);
   // this.databaseTableCreateScript = this.databaseTableService.LoadTableCreateScript(this.databaseTableName, this.databaseName);
    console.table(this.databaseTableCreateScript);
    this.databaseTableFkDependencies = this.databaseTableService.LoadTableFKeys(this.databaseTableName, this.databaseName);
    this.databaseTableKeyConstraints = this.databaseTableService.LoadTableKeyConstraints(this.databaseTableName, this.databaseName);
    this.databaseTableFragmentationDetails = this.databaseTableService.LoadTableFragmentationDetails(this.databaseTableName, this.databaseName);
    this.LoadTableDependencyTree();
    this.LoadTableDescription();
    this.LoadTableColumnsDetails();
    this.BreadCrumb();

    this._http.get<TableCreateScript>(this.baseUrl + "DatabaseTables/GetTableCreateScript", { params: { astrtableName: this.databaseTableName, astrdbName: this.databaseName } }).toPromise()
      .then(res => {
        this.databaseTableCreateScript = res;
        this.iblnShowTableCreateScript = true;
      });

    //public LoadTableCreateScript(astrtableName: string, astrdbName: string): Observable<TableCreateScript> {
    //  return this._http.get<TableCreateScript>(this.baseUrl + "DatabaseTables/GetTableCreateScript", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  //}
  } 
  EditTableMsDesciption($event): any
  {
    this.iblnShowEditBox = true; 
  }
  SaveTableMsDesciption($event): any
  {
    this.iblnShowEditBox = false;
    this.CreateOrUpdateTableDescription(this.databaseTableName, this.databaseName, this.tableDescription); 
  }
  CancelTableMsDesciption($event): any
  {
    this.tableDescription = this.tableDescriptionOld;
    this.iblnShowEditBox = false; 
  }
  EditGridRow(InputRowId: any)
  {
    this.databaseTableColumns[InputRowId].HideEdit = true;
  }
  SaveGridRow(InputRowId: any)
  {
    this.databaseTableColumns[InputRowId].HideEdit = false;
    this.CreateOrUpdateTableColumnDescription(this.databaseTableName, this.databaseName, this.databaseTableColumns[InputRowId].description, this.databaseTableColumns[InputRowId].columnname);
  }
  CancelGridRow(InputRowId: any)
  {
    this.databaseTableColumns[InputRowId].HideEdit = false;
  }
  samePageNevigation(databasetableName: any)
  {
    this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
      this.route.navigate(["/DatabaseTable", this.databaseName + "/" + databasetableName]));
  }
  private LoadTableDependencyTree()
  {
    this._http.get<any>(this.baseUrl + 'DatabaseTables/GetDependancyTree', { params: { astrtableName: this.databaseTableName, astrdbName: this.databaseName } }).toPromise()
      .then(res => {
        this.databaseTableDependencyTree = res.data;
      });
  }
  private LoadTableDescription()
  {
    this._http.get<any>(this.baseUrl + 'DatabaseTables/GetTableDescription', { params: { astrtableName: this.databaseTableName, astrdbName: this.databaseName } }).toPromise()
      .then(res => {
        this.databaseTableDescription = res;
        this.tableDescription = this.databaseTableDescription.desciption;
        this.tableDescriptionOld = this.tableDescription;
      });
  }
  private LoadTableColumnsDetails()
  {
    this._http.get<any>(this.baseUrl + 'DatabaseTables/GetAllTablesColumn', { params: { astrtableName: this.databaseTableName, astrdbName: this.databaseName } }).toPromise()
      .then(res => {
        this.databaseTableColumns = res;
        var count = 0;
        for (let Property of this.databaseTableColumns) { Property.id = count; Property.HideEdit = false; count++; }
        this.databaseTableColumns = res;
      });
  }
  private CreateOrUpdateTableDescription(astrtableName: string, astrdbName: string, astrDescription_Value: string)
  {
    this._http.get<boolean>(this.baseUrl + "DatabaseTables/CreateOrUpdateTableDescription",
      { params: { astrtableName: astrtableName, astrdbName: astrdbName, astrDescription_Value: astrDescription_Value } }).subscribe((result: any) => { }, error => console.error(error));;
  }
  private CreateOrUpdateTableColumnDescription(astrtableName: string, astrdbName: string, astrDescription_Value: string, astrColumnName: string)
  {
    this._http.get<boolean>(this.baseUrl + "DatabaseTables/CreateOrUpdateColumnDescription",
      { params: { astrtableName: astrtableName, astrdbName: astrdbName, astrDescription_Value: astrDescription_Value, astrColumnName: astrColumnName } }).subscribe((result: any) => { }, error => console.error(error));;
  }
  public BreadCrumb()
  {
    this.home = { label: 'Project', icon: 'pi pi-home', routerLink: "/" }; 
    this.items =
      [
         
        {

          label: 'User Databases',
          icon: "fa fa-folder",
        },
        {

          label: this.databaseName,
          icon: "fa fa-database fa-fw",
          routerLink: "/Database/" + this.databaseName
        },
        {

          label: "Tables",
          routerLink: "/DatabaseTables/" + this.databaseName,
          icon: "fa fa-folder"
        },
        {

          label: this.databaseTableName,
          icon: "fa fa-table fa-fw"

        }
      ];
  }
}

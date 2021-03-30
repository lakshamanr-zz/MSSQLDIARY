import { Component, Inject, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { ActivatedRoute } from '@angular/router'; 
import { HttpClient } from '@angular/common/http';
import { DatabaseTableService } from 'src/services/database-table.service';
import { TablePropertyInfo } from 'src/models/TablePropertyInfo';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-database-tables',
  templateUrl: './database-tables.component.html',
  styleUrls: ['./database-tables.component.css']
})
export class DatabaseTablesComponent implements OnInit {
 
  constructor(private ngxLoader: NgxUiLoaderService,public route:ActivatedRoute ,public databaseTableService: DatabaseTableService, public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string)
  {
    this.query="";
  } 
  public items: MenuItem[];
  public home: MenuItem;
  public query: any;
  public cols: any[];

  public searchInAll: boolean;
  public searchInTable: boolean;
  public searchInColumn: boolean;
  public searchInSSIS: boolean; 

  databaseName: string;
  databaseServerName: string;
  databaseTableName: string;
  databaseTables: TablePropertyInfo[];


  ngOnInit()
  {
    this.cols = [
      { field: 'istrFullName', header: 'istrFullName' },
      { field: 'istrValue', header: 'istrValue' }
    ];


    this.databaseName = this.route.snapshot.params.databaseDetails.split('/')[0];
    this.databaseServerName = this.route.snapshot.params.databaseDetails.split('/')[1];
    this.LoadDefaultDatabaseTables();
    this.searchInAll = true;
    this.BreadCrumb();
  }
  private LoadDefaultDatabaseTables()
  {
    this.ngxLoader.start();
    this._http.get<TablePropertyInfo[]>(this.baseUrl + "DatabaseTables/GetAllDatabaseTable", { params: { astrdbName: this.databaseName } })
      .toPromise()
      .then(res =>
      {
        this.databaseTables = res;
        this.ngxLoader.stop();
      }, error => this.ngxLoader.stop());
  }
  changeSuit(e)
  {
    this.searchInAll = false;
    this.searchInColumn = false
    this.searchInTable = false;
    this.searchInSSIS = false;
    this.query = "";
    this.LoadDefaultDatabaseTables();
    switch (e) {
      case "searchInAll":
        this.searchInAll = true;
        this.LoadDefaultDatabaseTables();
        break;
      case "searchInColumn":
        this.searchInColumn = true
        this.LoadDefaultDatabaseTables();
        break;
      case "searchInTable":
        this.searchInTable = true;
        this.LoadDefaultDatabaseTables();
        break;
       
      default:
        this.searchInAll = true;
        break;

    }

  }

  public BreadCrumb() {
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

          label:"Tables",
          icon: "fa fa-table fa-fw"

        }
      ];
  }
}

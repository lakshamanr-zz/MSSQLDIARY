import { Component, Inject, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ViewPropertyInfo } from '../../Models/ViewPropertyInfo';


@Component({
  selector: 'app-database-views',
  templateUrl: './database-views.component.html',
  styleUrls: ['./database-views.component.css']
})
export class DatabaseViewsComponent implements OnInit {

  public databaseName: string;
  public databaseServerName: string; 
  public cols: any[]; 
  public databaseViews: ViewPropertyInfo[]; 
  public items: MenuItem[];
  public home: MenuItem;


  constructor(private ngxLoader: NgxUiLoaderService, public route: ActivatedRoute,  public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string)
  {

  }

  ngOnInit()
  {

    this.cols = [
      { field: 'istrFullName', header: 'istrFullName' },
      { field: 'istrValue', header: 'istrValue' }
    ];
    this.databaseName = this.route.snapshot.params.databaseDetails.split('/')[0];
    this.databaseServerName = this.route.snapshot.params.databaseDetails.split('/')[1];
    this.loadDatabaseViews();


  }
  private loadDatabaseViews()
  {
    this.ngxLoader.start();
    this._http.get<ViewPropertyInfo[]>(this.baseUrl + "DatabaseView/GetAllViewsDetails", { params: { istrdbName: this.databaseName } })
      .toPromise()
      .then(res =>
      {
        this.databaseViews = res;
        for (let Property of this.databaseViews)
        {
          Property.istrNevigation = this.databaseName + "/" + Property.istrName;
          Property.istrFullName = Property.istrName;

        }
        this.ngxLoader.stop();
      }, error => this.ngxLoader.stop());
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

          label: "Views",
          icon: "fa fa-table fa-fw"

        }
      ];
  }
}

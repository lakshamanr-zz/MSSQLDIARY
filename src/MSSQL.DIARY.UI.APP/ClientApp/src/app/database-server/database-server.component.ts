import { Component, OnInit, Inject } from '@angular/core';
import { MenuItem } from 'primeng/api/menuitem';
import { DatabaseServerService } from 'src/services/database-server.service';
import { Observable } from 'rxjs';
import { error } from '@angular/compiler/src/util';
import { ServerProprty } from 'src/models/ServerProprty'
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-database-server-information',
  templateUrl: './database-server.component.html',
  styleUrls: ['./database-server.component.css']
})
export class DatabaseServerComponent implements OnInit {

  constructor(public databaseServerService: DatabaseServerService, public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string)
  {
    this.BreadCrumb();
    this._http.get<string>(this.baseUrl + "DatabaseServer/GetServerInformation").subscribe(result => {
      this.serverName = result;
    });
  }
  public menuItems: MenuItem[];
  public home: MenuItem;

  public serverName: string
  public databaseNames: Observable<string[]>
  public serverProprties: Observable<ServerProprty[]>
  public serverAdvenceProprties: Observable<ServerProprty[]>

  

  ngOnInit()
  {
    
    this.databaseNames = this.databaseServerService.GetDatabaseNames();
    this.serverProprties = this.databaseServerService.GetServerProperties();
    this.serverAdvenceProprties = this.databaseServerService.GetAdvancedServerSettings();
  }
  public BreadCrumb()
  {
    this.home = { label: 'Project', icon: 'pi pi-home', routerLink: "/" };
    this.menuItems =
      [

      ]; 
  }
}

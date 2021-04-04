import { Component, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http"; 
import { Observable } from 'rxjs';
import { AuthorizeService } from '../../api-authorization/authorize.service';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { DatabaseName } from "../../Models/DatabaseName";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',

})
export class HomeComponent
{

  public istrServerNameList: string[];
  public lstDataBaseName: DatabaseName[];
  public loggedin: boolean;
  public isAuthenticated: Observable<boolean>; 
  public selectedDatabaseName: any;
  public selectedServerName: any;
  constructor(private ngxService: NgxUiLoaderService, private authorizeService: AuthorizeService, private http: HttpClient, @Inject('BASE_URL') public baseUrl: string)
  {
    this.isAuthenticated = this.authorizeService.isAuthenticated(); 
    this.LoadServerList(); 
  }


  ServerchangeValue($event) {
     
    this.selectedServerName = $event.target.value;
    this.LoadDatabaseNames(this.selectedServerName);
  }
  databaseChangeEvent($event)
  {
    this.selectedDatabaseName = $event.target.value;
    this.http.get<any>(this.baseUrl + "DatabaseServer/SetDefaultDatabase", { params: { astrServerName: this.selectedServerName, astrDatabaseName: this.selectedDatabaseName} }).subscribe(result =>
    {
        window.location.reload();
    });
  }

  private LoadServerList() {
    this.http.get<string[]>(this.baseUrl + 'DatabaseServer/GetServerNameList').subscribe(result => { 
      this.istrServerNameList = result; 
    }, error => console.log(error));
  }

  private LoadDatabaseNames(selectedServerName:any) {
    this.http.get<DatabaseName[]>(this.baseUrl + 'DatabaseServer/GetDatabaseNamesByServername', { params: { astrServerName: selectedServerName} }).subscribe(result => {
      this.lstDataBaseName = result;
    }, error => console.log(error));
  }


   
}

import { Component, OnInit } from '@angular/core';
import { SchemaReferanceInfo } from '../../models/SchemaReferanceInfo';
import { FileInfomration } from '../../models/FileInfomration';
import { ServerProprty, PropertyInfo } from '../../models/ServerProprty';
import { Observable } from 'rxjs';
import { DatabaseService } from '../../services/database.service';
import { ActivatedRoute } from '@angular/router';
import { MenuItem } from 'primeng/api/menuitem';

@Component({
  selector: 'app-database',
  templateUrl: './database.component.html',
  styleUrls: ['./database.component.css']
})
export class DatabaseComponent implements OnInit
{
  public GetDatabaseObjectTypes: Observable<string[]>;
  public GetdbPropertValues: Observable<PropertyInfo[]>;
  public GetdbOptionValues: Observable<PropertyInfo[]>;
  public GetdbFilesDetails:  Observable<FileInfomration[]>;
  public schemReferance: Observable<SchemaReferanceInfo[]>;

  databaseName: string;
  databaseServerName: string;

  public menuItems: MenuItem[];
  public home: MenuItem;
  constructor(public dbService: DatabaseService, private route: ActivatedRoute)
  {
    this.BreadCrumb();
  }

  ngOnInit()
  {
    this.databaseName = this.route.snapshot.params.databaseDetails.split('/')[0];
    this.databaseServerName = this.route.snapshot.params.databaseDetails.split('/')[1];
    this.GetdbPropertValues = this.dbService.GetdbPropertValues(this.databaseName);
    this.GetDatabaseObjectTypes = this.dbService.GetDatabaseObjectTypes();
    this.GetdbOptionValues = this.dbService.GetdbOptionValues(this.databaseName);
    this.GetdbFilesDetails = this.dbService.GetdbFilesDetails(this.databaseName);
     
  }
  public BreadCrumb() {
    this.home = { label: 'Project', icon: 'pi pi-home', routerLink: "/" };
    this.menuItems =
      [

      ];
  }

}

import { Component, OnInit, Inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { LeftMenuTreeViewJson } from '../../models/LeftMenuTreeViewJson';
import { SchemaEnums } from 'src/models/SchemaEnums';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.css']
})
export class LeftMenuComponent implements OnInit {

  @Input() objectexplorercontainst: any;
  databaseName: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') public baseUrl: string, private route: Router) {
    this.databaseName = "";
  }
  getNodeData(data: LeftMenuTreeViewJson) {
    this.databaseName = data.link.split('/')[4];

    if (data.mdaIcon == "User Database") {
      return;
    }
    switch (data.SchemaEnums) {

      case SchemaEnums.ProjectInfo || SchemaEnums.DatabaseServer:
        {
          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["DatabaseServer"]));
        }

        break;
      case SchemaEnums.AllDatabase:
        {
          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/Database", data.text + "/" + data.link.split('/')[2]]));
        }
        break;
      case SchemaEnums.AllTable:
        {
          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseTables", this.databaseName + "/" + data.link]));
        }
        break;
      case SchemaEnums.Table:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseTable", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllViews:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseViews", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.Views:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseView", this.databaseName + "/" + data.text]));
        }
        break;

      case SchemaEnums.AllStoreprocedure:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/StoreProcedures", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.Storeprocedure:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/StoreProcedureDetails", this.databaseName + "/" + data.text + "/" + data.link]));
        }
        break;
      case SchemaEnums.AllTableValueFunction:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/TableValueFunctions", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.TableValueFunction:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/TableValueFunctionDetails", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllScalarValueFunctions:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/ScalarFunctions", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.ScalarValueFunctions:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/ScalarFunctionDetails", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllAggregateFunciton:
        {

        }
        break;
      case SchemaEnums.AggregateFunciton:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/StoreProcedureDetails", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllTriggers:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseTriggers", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.Triggers:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/DatabaseTriggerDetails", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllUserDefinedDataType:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/UserDefinedDataTypes", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.UserDefinedDataType:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/UserDefinedDataTypeDetails", this.databaseName + "/" + data.text]));
        }
        break;
      case SchemaEnums.AllXMLSchemaCollection:
        {

          this.route.navigateByUrl('/', { skipLocationChange: true }).then(() =>
            this.route.navigate(["/XmlSchemaCollections", this.databaseName + "/" + data.text]));
        }
        break;

    }
  }
  ngOnInit() {
  }


}

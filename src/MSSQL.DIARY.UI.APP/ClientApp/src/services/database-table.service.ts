import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TablePropertyInfo } from 'src/models/TablePropertyInfo' 
import { TableIndexInfo } from 'src/models/TableIndexInfo';
import { TableCreateScript } from 'src/models/TableCreateScript';
import { Tabledependencies } from 'src/models/Tabledependencies';
import { TableColumns } from '../models/TableColumns';
import { Ms_Description } from '../models/Ms_Description';
import { TableFKDependency } from 'src/models/TableFKDependency';
import { TableKeyConstraint } from '../models/TableKeyConstraint';
import { TableFragmentationDetails } from 'src/models/TableFragmentationDetails';
@Injectable({
  providedIn: 'root'
})
export class DatabaseTableService {

  constructor(public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string)  {
  }
  public LoadDatabaseTables(databaseName: string): Observable<TablePropertyInfo[]> { 
    return this._http.get<TablePropertyInfo[]>(this.baseUrl + "DatabaseTables/GetAllDatabaseTable", { params: { astrdbName: databaseName } }).pipe(map(u => u));
  } 
  public LoadTableDependencies(astrtableName: string, astrdbName: string): Observable<Tabledependencies[]> {
    return this._http.get<Tabledependencies[]>(this.baseUrl + "DatabaseTables/GetAllTabledependencies", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  } 
  public LoadTableIndexes(astrtableName: string, astrdbName: string): Observable<TableIndexInfo[]>
  {
    return this._http.get<TableIndexInfo[]>(this.baseUrl + "DatabaseTables/LoadTableIndexes", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }

  public GetAllTabledependencies(astrtableName: string, astrdbName: string): Observable<Tabledependencies[]> {
    return this._http.get<Tabledependencies[]>(this.baseUrl + "DatabaseTables/GetAllTabledependencies", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
  public LoadTableColumnsDetails(astrtableName: string, astrdbName: string): Observable<TableColumns[]> {
    return this._http.get<TableColumns[]>(this.baseUrl + "DatabaseTables/GetAllTablesColumn", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
  public LoadTableDescriptions(astrtableName: string, astrdbName: string): Observable<Ms_Description> {
    return this._http.get<Ms_Description>(this.baseUrl + "DatabaseTables/GetTableDescription", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
  public LoadTableFKeys(astrtableName: string, astrdbName: string): Observable<TableFKDependency[]> {
    return this._http.get<TableFKDependency[]>(this.baseUrl + "DatabaseTables/GetAllTableForeignKeys", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
  public LoadTableKeyConstraints(astrtableName: string, astrdbName: string): Observable<TableKeyConstraint[]> {
    return this._http.get<TableKeyConstraint[]>(this.baseUrl + "DatabaseTables/GetTableKeyConstraints", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
  public CreateOrUpdateColumnDescription(astrtableName: string, astrdbName: string, astrDescription_Value: string, astrColumnName: string): Observable<boolean> {
    return this._http.get<boolean>(this.baseUrl + "DatabaseTables/CreateOrUpdateColumnDescription", { params: { astrtableName: astrtableName, astrdbName: astrdbName, astrDescription_Value: astrDescription_Value, astrColumnName: astrColumnName} }).pipe(map(u => u));
  }
  public CreateOrUpdateTableDescription(astrtableName: string, astrdbName: string, astrDescription_Value: string): Observable<boolean> {
    return this._http.get<boolean>(this.baseUrl + "DatabaseTables/CreateOrUpdateTableDescription", { params: { astrtableName: astrtableName, astrdbName: astrdbName, astrDescription_Value: astrDescription_Value} }).pipe(map(u => u));
  }
  public LoadTableDependencyTree(astrtableName: string, astrdbName: string): any {
    this._http.get<any>(this.baseUrl + 'DatabaseTables/GetDependancyTree', { params: { astrtableName: astrtableName, astrdbName: astrdbName } } ).toPromise()
  }
  public LoadTableFragmentationDetails(astrtableName: string, astrdbName: string): Observable<TableFragmentationDetails[]> {
    return this._http.get<TableFragmentationDetails[]>(this.baseUrl + "DatabaseTables/TableFragmentationDetails", { params: { astrtableName: astrtableName, astrdbName: astrdbName } }).pipe(map(u => u));
  }
}

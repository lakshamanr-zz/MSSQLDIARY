import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ServerProprty, PropertyInfo } from '../models/ServerProprty';
import { FileInfomration } from '../models/FileInfomration';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  constructor(public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {
  }
  public GetDatabaseObjectTypes(): Observable<string[]>
  {
    return this._http.get<string[]>(this.baseUrl +"Database/GetDatabaseObjectTypes").pipe(map(u => u));
  }
  public GetdbPropertValues(dbName: string): Observable<PropertyInfo[]>
  {
    return this._http.get<PropertyInfo[]>(this.baseUrl + "Database/GetdbPropertValues", { params: { istrdbName: dbName} }).pipe(map(u => u));
  }
  public GetdbOptionValues(dbName: string): Observable<PropertyInfo[]> {
    return this._http.get<PropertyInfo[]>(this.baseUrl + "Database/GetdbOptionValues", { params: { istrdbName: dbName } }).pipe(map(u => u));
  }
  public GetdbFilesDetails(dbName: string): Observable<FileInfomration[]> {
    return this._http.get<FileInfomration[]>(this.baseUrl + "Database/GetdbFilesDetails", { params: { istrdbName: dbName } }).pipe(map(u => u));
  } 
}

import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServerProprty } from 'src/Models/ServerProprty'
@Injectable({
  providedIn: 'root'
})
export class DatabaseServerService {

  constructor(public _http: HttpClient, @Inject('BASE_URL') public baseUrl: string, public ngxLoader: NgxUiLoaderService) {
  }
  public GetServerInformation(): Observable<string> {

    return this._http.get<string>(this.baseUrl + "DatabaseServer/GetServerInformation").pipe(map(u => u));
  }

  public GetDatabaseNames(): Observable<string[]>
  {
    return this._http.get<string[]>(this.baseUrl + "DatabaseServer/GetDatabaseNames").pipe(map(u =>u));
  }
  public GetServerProperties(): Observable<ServerProprty[]>
  {
    return this._http.get<ServerProprty[]>(this.baseUrl + "DatabaseServer/GetServerProperties").pipe(map(u =>u));
  }
  public GetAdvancedServerSettings(): Observable<ServerProprty[]>
  {
    return this._http.get<ServerProprty[]>(this.baseUrl + "DatabaseServer/GetAdvancedServerSettings").pipe(map(u => u));
  }
}

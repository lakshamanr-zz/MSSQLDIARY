import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http'; 

@Injectable({
  providedIn: 'root'
})
//DatabaseServerService
export class databaseServerObjectExplorer {

  constructor(private http: HttpClient, @Inject('BASE_URL') public baseUrl: string) 
  {

  }
  public GetObjectExploreMenu():any
  { 
    this.http.get<any>(this.baseUrl + "ObjectExplorer").subscribe(result =>
    {
      return result;

    },error=>console.log(console.error())
    );
  }
}

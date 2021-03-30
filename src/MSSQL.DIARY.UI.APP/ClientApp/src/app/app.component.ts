
import { Component, ChangeDetectionStrategy, ViewChild, Inject, AfterViewInit, OnInit  } from '@angular/core';
import { SplitComponent, SplitAreaDirective } from 'angular-split';
import { Observable } from 'rxjs';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit, OnInit {
  title = 'app';
  @ViewChild('split', { static: false }) split: SplitComponent;
  @ViewChild('area1', { static: false }) area1: SplitAreaDirective;
  @ViewChild('area2', { static: false }) area2: SplitAreaDirective;
  direction: string = 'horizontal';
  isAuthenticated: Observable<boolean>;
  objectexplorercontainst: any;
  currentdatabase: string;
  iblnSpitterState: boolean;
  istrServerNameList: string[];
  istrDataBaseName: string[];
  constructor(private ngxService: NgxUiLoaderService, private authorizeService: AuthorizeService, private http: HttpClient, @Inject('BASE_URL') public baseUrl: string, private route: Router) {
    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.currentdatabase = "";

  }
  ngOnInit(): void
  {
    this.LoadDatabaseObjectExporer();
    //this.LoadServerList()
    //this.LoadDatabaseNames();
  }

  ngAfterViewInit(): void
  {

  }
  sizes =
    {
      percent:
      {
        area1: 30,
        area2: 70,
      },
      pixel:
      {
        area1: 120,
        area2: '*',
        area3: 160,

        dragEnd(unit, { sizes }) {
          if (unit === 'percent') {
            this.sizes.percent.area1 = sizes[0];
            this.sizes.percent.area2 = sizes[1];
          }
          else if (unit === 'pixel') {
            this.sizes.pixel.area1 = sizes[0];
            this.sizes.pixel.area2 = sizes[1];
            this.sizes.pixel.area3 = sizes[2];
          }
        }
      }

    }

  private LoadDatabaseObjectExporer() {
    this.ngxService.startLoader('loader-01');
    this.http.get<any>(this.baseUrl + "DatabaseObjectExplorer",
      {
        params: {
          DatabaseServerName: ""
        }
      }).subscribe(result => {
        if (result == null) {
          this.objectexplorercontainst = null;
          this.ngxService.stopLoader('loader-01');
          window.location.reload();
        }
        else {
          this.objectexplorercontainst = result;
          this.ngxService.stopLoader('loader-01');
        }
      }, error => {
          this.ngxService.stopLoader('loader-01');
          console.log(console.error());
        });
  }

  changeValue($event) {
    this.ngxService.startLoader('loader-01');
    this.currentdatabase = $event.target.value;
    this.http.get<any>(this.baseUrl + "DatabaseObjectExplorer",
      {
        params:
        {
          DatabaseServerName: $event.target.value,
          astrDatabaseName: ''
        }
      }).subscribe(result => {

        if (result == null) {
          this.objectexplorercontainst = null;
          this.ngxService.stopLoader('loader-01');

        }
        else {
          this.objectexplorercontainst = result;
          this.http.get<string[]>(this.baseUrl + 'DatabaseServer/GetDatabaseNames').subscribe(result => {
            this.istrDataBaseName = result;
          }, error1 => console.log(error1));
          this.ngxService.stopLoader('loader-01');
        }
      }, error => {
          this.ngxService.stopLoader('loader-01');
          console.log(console.error())
        });
  }

  LoadObjectExporer($event) {
    this.ngxService.startLoader('loader-01');
    this.http.get<any>(this.baseUrl + "DatabaseObjectExplorer",
      {
        params:
        {
          DatabaseServerName: this.currentdatabase,
          astrDatabaseName: $event.target.value
        }
      }).subscribe(result => {

        if (result == null) {
          this.objectexplorercontainst = null;
          this.ngxService.stopLoader('loader-01');
          window.location.reload();
        }
        else {
          this.objectexplorercontainst = result;

          this.ngxService.stopLoader('loader-01');
        }
      }, error => {
          this.ngxService.stopLoader('loader-01');
          console.log(console.error())
        });
  }
  SplitterEventHandler(type: string, e:
    { gutterNum: number, sizes: Array<number> })
  {
    switch (type)
    {
      case "gutterDblClick":
        {
          this.iblnSpitterState = !this.iblnSpitterState;

        }
        break;
      case "gutterClick":
        {
          this.iblnSpitterState = !this.iblnSpitterState;

        }
        break;
      case "dragEnd":
        {
          //alert("Drage end");
        }
        break;
      case "dragStart":
        {
          //alert("Drage start");
        }
        break;
    }
  }

  private LoadServerList()
  {
    this.http.get<string[]>(this.baseUrl + 'DatabaseServer/GetServerNameList').subscribe(result =>
    {
      this.istrServerNameList = result;
    }, error => console.log(error));
  }

  private LoadDatabaseNames()
  {
    this.http.get<string[]>(this.baseUrl + 'DatabaseServer/GetDatabaseNames').subscribe(result =>
    {
      this.istrDataBaseName = result;
    }, error => console.log(error));
  }
}


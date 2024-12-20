import { RouterModule} from "@angular/router";
import { CommonModule } from "@angular/common";
import { provideHttpClient } from "@angular/common/http";
import { PatientService } from "./services/patient/patient.service"
import {appRoutes} from "./app.routes";
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

@NgModule({
    declarations:[
    ],
    imports:[
        CommonModule,
        RouterModule.forRoot(appRoutes),
        BrowserModule,
        BrowserAnimationsModule],
    providers:[PatientService,provideHttpClient()],
    bootstrap:[]
})
export class AppModule{}
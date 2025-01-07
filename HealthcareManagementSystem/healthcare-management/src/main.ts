import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { AppComponent } from '../src/app/app.component';
import {provideHttpClient} from '@angular/common/http';
import {ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {appRoutes} from '../src/app/app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(appRoutes),
    BrowserAnimationsModule,
    ReactiveFormsModule,
    [provideHttpClient()], provideAnimationsAsync()
  ]
}).catch(err => console.error(err));

import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { CompletedCalibrationComponent } from './submitGroup/calibration.component';
import { CalibrationService } from 'src/services/calibration.service';
import { ViewAllCalibrations } from './viewAllCalibrations/view-all-calibrations.component';
import { ViewSingleCalibrationComponent } from './viewSingleCalibration/view-calibration.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent,
    CompletedCalibrationComponent,
    ViewAllCalibrations,
    ViewSingleCalibrationComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
  ],
  providers: [CalibrationService],
  bootstrap: [AppComponent]
})
export class AppModule { }

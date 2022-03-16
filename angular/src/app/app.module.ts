import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { CalibrationService } from 'src/services/calibration.service';
import { ViewAllCalibrations } from './viewAllCalibrations/view-all-calibrations.component';
import { ViewSingleCalibrationComponent } from './viewSingleCalibration/view-calibration.component';
import { appRoutes } from './routes';
import { GroupCalibrationComponent } from './viewGroupCalibration/view-group-calibration.component';

@NgModule({
  declarations: [
    AppComponent,
    ViewAllCalibrations,
    ViewSingleCalibrationComponent,
    GroupCalibrationComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
  ],
  providers: [CalibrationService],
  bootstrap: [AppComponent]
})
export class AppModule { }

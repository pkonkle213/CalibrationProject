import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { appRoutes } from './routes';
import { AuthService } from 'src/services/auth.service';
import { CalibrationService } from 'src/services/calibration.service';
import { CreateCalibration } from './createCalibration/create-calibration.component';
import { GroupCalibrationComponent } from './viewGroupCalibration/view-group-calibration.component';
import { LoginComponent } from './login/login.component';
import { ViewAllCalibrations } from './viewAllCalibrations/view-all-calibrations.component';
import { ViewSingleCalibrationComponent } from './viewSingleCalibration/view-calibration.component';
import { UserService } from 'src/services/user.service';
import { CreateUserComponent } from './createUser/create-user.component'
import { ViewStatsComponent } from './viewStats/view-stats.component';
import { StatsService } from 'src/services/statistics.service';

@NgModule({
  declarations: [
    AppComponent,
    ViewAllCalibrations,
    ViewSingleCalibrationComponent,
    GroupCalibrationComponent,
    LoginComponent,
    CreateCalibration,
    CreateUserComponent,
    ViewStatsComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
  ],
  providers: [
    CalibrationService,
    AuthService,
    UserService,
    StatsService,
  ],
  bootstrap: [
    AppComponent,
  ]
})
export class AppModule { }

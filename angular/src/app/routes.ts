import { Routes } from "@angular/router";
import { CreateCalibration } from "./createCalibration/create-calibration.component";
import { LoginComponent } from "./login/login.component";
import { CreateUserComponent } from "./createUser/create-user.component";
import { ViewAllCalibrations } from "./viewAllCalibrations/view-all-calibrations.component";
import { GroupCalibrationComponent } from "./viewGroupCalibration/view-group-calibration.component";
import { ViewSingleCalibrationComponent } from "./viewSingleCalibration/view-calibration.component";
import { ViewStatsComponent } from "./viewStats/view-stats.component";
import { CreateFormComponent } from "./createFormQuestion/create-form-question.component"

export const appRoutes:Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'view', component: ViewAllCalibrations },
    { path: 'view/:id', component: ViewSingleCalibrationComponent },
    { path: 'group/:id', component: GroupCalibrationComponent },
    { path: 'createCalibration', component: CreateCalibration },
    { path: 'editUsers', component: CreateUserComponent },
    { path: 'stats', component: ViewStatsComponent },
    { path: 'form', component: CreateFormComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
]
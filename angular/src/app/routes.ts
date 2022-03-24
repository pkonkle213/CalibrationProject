import { Routes } from "@angular/router";
import { CreateCalibration } from "./createCalibration/create-calibration.component";
import { EditUsersComponent } from "./editUsers/edit-users.component";
import { LoginComponent } from "./login/login.component";
import { ViewAllCalibrations } from "./viewAllCalibrations/view-all-calibrations.component";
import { GroupCalibrationComponent } from "./viewGroupCalibration/view-group-calibration.component";
import { ViewSingleCalibrationComponent } from "./viewSingleCalibration/view-calibration.component";

export const appRoutes:Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'view', component: ViewAllCalibrations },
    { path: 'view/:id', component: ViewSingleCalibrationComponent },
    { path: 'group/:id', component: GroupCalibrationComponent },
    { path: 'create', component: CreateCalibration },
    { path: 'edit', component: EditUsersComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' },
]
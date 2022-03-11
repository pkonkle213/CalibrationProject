import { Routes } from "@angular/router";
import { ViewAllCalibrations } from "./viewAllCalibrations/view-all-calibrations.component";
import { ViewSingleCalibrationComponent } from "./viewSingleCalibration/view-calibration.component";

export const appRoutes:Routes = [
    { path: 'view', component: ViewAllCalibrations },
    { path: 'view/:id', component: ViewSingleCalibrationComponent },
    { path: '', redirectTo: '/view', pathMatch: 'full' },
    
]
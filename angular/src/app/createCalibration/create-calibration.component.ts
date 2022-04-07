import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ICalibration } from "src/interfaces/calibration";
import { CalibrationService } from "src/services/calibration.service";

@Component({
    templateUrl: 'create-calibration.component.html',
    styleUrls: ['create-calibration.component.css'],
})

export class CreateCalibration {
    options:any;
    calibration:ICalibration = {
        id:0,
        repFirstName: "",
        repLastName: "",
        groupScoreEarned: 0,
        groupScorePossible: 0,
        contactChannel: "",
        calibrationDate: new Date('8/20/1987'),
        contactId: "",
        isOpen: true,
    }

    constructor(private _calibrationService:CalibrationService, private router:Router) {

    }

    ngOnInit() {
        this._calibrationService.getAllContactTypes().subscribe(data => {
            this.options = data;
        })
    }

    CreateCalibration() {
        this._calibrationService.createCalibration(this.calibration).subscribe(() => {
            this.router.navigate(['/view/']);
        });
    }
}
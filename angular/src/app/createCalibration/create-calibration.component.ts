import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { ICalibration } from "src/interfaces/calibration";
import { CalibrationService } from "src/services/calibration.service";
import { FormService } from "src/services/form.service";

@Component({
    templateUrl: 'create-calibration.component.html',
    styleUrls: ['create-calibration.component.css'],
})

export class CreateCalibration {
    options:any;
    forms:any;
    calibration:ICalibration = {
        id:0,
        repFirstName: "",
        repLastName: "",
        leaderUserId: 1,
        groupScoreEarned: 0,
        groupScorePossible: 0,
        contactChannelId: 0,
        calibrationDate: new Date('8/20/1987'),
        contactId: "",
        isOpen: true,
        formId: 0,
    }

    constructor(private _calibrationService:CalibrationService, private _formService:FormService, private router:Router) {

    }

    ngOnInit() {
        this._calibrationService.getAllContactTypes().subscribe(data => {
            this.options = data;
        })

        this._formService.getActiveForms().subscribe((data) => {
            this.forms = data;
        })
    }

    CreateCalibration() {
        this._calibrationService.createCalibration(this.calibration).subscribe(() => {
            this.router.navigate(['/view/']);
        });
    }
}
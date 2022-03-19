import { Component } from "@angular/core";
import { ICalibration } from "src/interfaces/calibration";

@Component({
    templateUrl: 'create-calibration.component.html',
    styleUrls: ['create-calibration.component.css'],
})

export class CreateCalibration {
    calibration:ICalibration = {
        id:0,
        repFirstName: "",
        repLastName: "",
        groupScoreEarned: 0,
        groupScorePossible: 0,
        contactChannel: "",
        calibrationDate: new Date('8/20/1987'),
        contactId: "",
        isActive: true,
        indivPointsEarned: 0,
        indivPointsPossible: 0,
    }
}
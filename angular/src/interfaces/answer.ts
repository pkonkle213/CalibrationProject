import { IDetail } from "./detail";

export interface IAnswer {
    participantId: number;
    details: IDetail[];
}
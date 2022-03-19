import { IOption } from "./calibrationOptions";

export interface IQuestion {
    id: number,
    questionText: string;
    isCategory: boolean;
    pointsPossible: number;
    options:IOption[];
}
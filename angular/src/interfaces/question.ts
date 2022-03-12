import { IOption } from "./options";

export interface IQuestion {
    id: number,
    questionText: string;
    isCategory: boolean;
    pointsPossible: number;
    options:IOption[];
}
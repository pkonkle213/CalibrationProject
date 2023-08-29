import { IOption } from "./option";

export interface IQuestion {
    id: number,
    formId: number,
    questionText: string,
    isCategory: boolean,
    pointsPossible: number,
    formPosition:number,
    options: IOption[],
}
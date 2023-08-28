import { IOption } from "./calibrationOption";

export interface ISendQuestion {
    id: number,
    formId: number,
    questionText: string,
    pointsPossible: number,
    formPosition:number,
    options: IOption[],
}
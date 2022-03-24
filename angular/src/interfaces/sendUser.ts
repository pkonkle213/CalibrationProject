export interface ISendUser {
    username:string,
    password:string,
    confirmPassword?:string,
    role?: string,
    team?: string,
    first?: string,
    last?: string
}
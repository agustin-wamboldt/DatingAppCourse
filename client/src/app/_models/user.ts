export interface User { // interface in TS => "this is a type of ____"
    userName: string;
    token: string;
    photoUrl: string;
    knownAs: string;
    gender: string;
    roles: string[];
}
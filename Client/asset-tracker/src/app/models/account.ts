export interface Account{
    profileId:string;
    accountId: string;
    type?:string | null;
    balance?: number | null;
    institution?: string | null;
}
export interface Debt {
    name: string;
    email: string;
    amount: number;
    borrowingDate: Date;
    deadline: Date;
    reason: string;
    isUserAccount: boolean;
}

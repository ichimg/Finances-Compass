export interface Debt {
    name: string;
    email: string;
    amount: number;
    borrowingDate: Date;
    deadline: Date;
    reason: string;
    status: string;
    isPaid: boolean;
    isUserAccount: boolean;
}

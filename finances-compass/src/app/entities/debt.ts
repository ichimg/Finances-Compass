export interface Debt {
    firstName: string;
    lastName: string;
    email: string;
    amount: string;
    borrowingDate: Date;
    deadline: Date;
    reason: string;
    status: string;
    isPaid: boolean;
    isUserAccount: boolean;
}

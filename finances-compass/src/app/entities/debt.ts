export interface Debt {
    guid: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    amount: string;
    borrowingDate: Date;
    deadline: Date;
    reason: string;
    status: string;
    isPaid: boolean;
    isUserAccount: boolean;
    eurExchangeRate: string;
    usdExchangeRate: string;
}

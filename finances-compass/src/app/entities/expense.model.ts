import { Category } from "./category.model";

export interface Expense {
    guid: string;
    amount: string;
    date: Date;
    category: string;
    note: string;
}
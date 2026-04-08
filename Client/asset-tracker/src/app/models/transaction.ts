export interface Transaction {
  accountId: string;
  transactionId: string;
  vendor?: string | null;
  amount?: number | null;
  date?: string | null; 
}
export interface Transaction {
  // Keys
  accountId: string;
  transactionId: string;

  // Core
  amount?: number | null;
  name ?: string | null;
  isoCurrencyCode?: string | null;
  merchantName?: string | null;
  merchantEntityId?: string | null;
  logoUrl?: string | null;

  // Dates
  date?: string | null;             
  authorizedDate?: string | null;

  // Status
  pending?: boolean | null;
  pendingTransactionId?: string | null;

  // Category
  categoryPrimary?: string | null;
  categoryDetailed?: string | null;
  categoryConfidence?: string | null;
  categoryIconUrl?: string | null;

  // Payment
  paymentChannel?: string | null;
}
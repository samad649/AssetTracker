export interface Account {
  profileId: string;
  accountId: string;
  itemId: string;
  name: string;
  mask: string;
  type: string;
  subtype: string;
  currentBalance?: number | null;
  availableBalance?: number | null;
  lastUpdated: string;
  institutionName: string;
}
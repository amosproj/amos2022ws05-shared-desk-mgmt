export interface IUser {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  company: string;
  isAdmin: boolean;
  isApproved: boolean;
  isMarkedForDeletion: boolean;
  selected?: boolean;
}

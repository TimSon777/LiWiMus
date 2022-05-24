import axios from "../shared/services/Axios";
import { Transaction } from "./types/Transaction";
import { UpdateTransactionDto } from "./types/UpdateTransactionDto";
import { FilterOptions } from "../shared/types/FilterOptions";
import { PaginatedData } from "../shared/types/PaginatedData";
import { CreateTransactionDto } from "./types/CreateTransactionDto"


const TransactionService = {
  getTransactions: async (options: FilterOptions<Transaction>) => {
    const response = await axios.get('/transactions', {
      params: options,
    })
    return response.data as PaginatedData<Transaction>
  },
  get: async (id: string) => {
    const response = await axios.get(`/transactions/${id}`);
    return response.data as Transaction;
  },

  save: async (transaction: CreateTransactionDto) => {
    const response = await axios.post(`/transactions`, transaction);
    return response.data as Transaction;
  },
  
  update: async (dto: UpdateTransactionDto) => {
    const response = await axios.patch(`/transactions`, dto);
    return response.data as Transaction;
  },
};

export default TransactionService;

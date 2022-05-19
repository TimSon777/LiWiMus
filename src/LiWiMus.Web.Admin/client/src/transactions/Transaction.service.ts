import axios from "../shared/services/Axios";
import { Transaction } from "./types/Transaction";
import { UpdateTransactionDto } from "./types/UpdateTransactionDto";

const TransactionService = {
  get: async (id: string) => {
    const response = await axios.get(`/transactions/${id}`);
    return response.data as Transaction;
  },

  update: async (dto: UpdateTransactionDto) => {
    const response = await axios.patch(`/transactions`, dto);
    return response.data as Transaction;
  },
};

export default TransactionService;

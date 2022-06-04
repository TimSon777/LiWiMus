import { Transaction } from "./types/Transaction";
import { UpdateTransactionDto } from "./types/UpdateTransactionDto";
import { FilterOptions } from "../shared/types/FilterOptions";
import { PaginatedData } from "../shared/types/PaginatedData";
import { CreateTransactionDto } from "./types/CreateTransactionDto";
import { useAxios } from "../shared/hooks/Axios.hook";

export const useTransactionService = () => {
  const axios = useAxios("/transactions");

  const getTransactions = async (options: FilterOptions<Transaction>) => {
    const response = await axios.get("", {
      params: options,
    });
    return response.data as PaginatedData<Transaction>;
  };
  
  const search = async () => {
    const response = await axios.get("",);
    return response
  }

  const get = async (id: string) => {
    const response = await axios.get(`/${id}`);
    return response.data as Transaction;
  };

  const save = async (transaction: CreateTransactionDto) => {
    const response = await axios.post(``, transaction);
    return response.data as Transaction;
  };

  const update = async (dto: UpdateTransactionDto) => {
    const response = await axios.patch(``, dto);
    return response.data as Transaction;
  };

  return { getTransactions, get, save, update };
};

import React, { useEffect, useState } from "react";
// @ts-ignore
import dateFormat from "dateformat";
import { Link as RouterLink, useParams } from "react-router-dom";
import { useNotifier } from "../../shared/hooks/Notifier.hook";
import Loading from "../../shared/components/Loading/Loading";
import NotFound from "../../shared/components/NotFound/NotFound";
import { Grid, Link, Stack, Typography } from "@mui/material";
import ReadonlyInfo from "../../shared/components/InfoItem/ReadonlyInfo";
import InfoCard from "../../shared/components/InfoCard/InfoCard";
import { Transaction } from "../types/Transaction";
import TransactionService from "../Transaction.service";
import TransactionInfoEditor from "../components/TransactionInfoEditor/TransactionInfoEditor";

export default function TransactionDetailsPage() {
  const { id } = useParams() as { id: string };
  const [transaction, setTransaction] = useState<Transaction>();
  const [loading, setLoading] = useState(true);
  const { showError } = useNotifier();

  useEffect(() => {
    TransactionService.get(id)
      .then((transaction) => {
        setTransaction(transaction);
      })
      .catch((error) => showError(error))
      .then(() => setLoading(false));
  }, [id]);

  if (loading) {
    return <Loading />;
  }

  if (!transaction) {
    return <NotFound />;
  }

  return (
    <>
      <Typography variant={"h3"} component={"div"}>
        Transaction
      </Typography>

      <Grid
        container
        spacing={10}
        justifyContent={"space-around"}
        sx={{ mb: 10 }}
      >
        <Grid item xs={12} md={8} lg={4}>
          <Stack direction={"column"} spacing={3} alignItems={"end"}>
            <ReadonlyInfo name={"ID"} value={transaction.id} />
            <ReadonlyInfo
              name={"User"}
              value={
                <Link
                  component={RouterLink}
                  to={`/admin/users/${transaction.user.id}`}
                  underline="none"
                  color={"secondary"}
                >
                  {transaction.user.userName}
                </Link>
              }
            />
            <ReadonlyInfo
              name={"Created at"}
              value={`${dateFormat(
                new Date(transaction.createdAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <ReadonlyInfo
              name={"Modified at"}
              value={`${dateFormat(
                new Date(transaction.modifiedAt),
                "dd.mm.yyyy, HH:MM"
              )}`}
            />
            <TransactionInfoEditor
              id={id}
              dto={{ description: transaction.description }}
              setDto={(dto) => {
                setTransaction({ ...transaction, ...dto });
              }}
            />
          </Stack>
        </Grid>
        <Grid item xs={12} md={8} lg={4}>
          <Stack spacing={4}>
            <InfoCard
              title={"Amount"}
              value={(+transaction.amount).toFixed(2)}
            />
          </Stack>
        </Grid>
      </Grid>
    </>
  );
}

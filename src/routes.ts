import { ApController } from "./controller/apController";

export const Routes = [
  {
    method    : "get",
    route     : "/aps",
    controller: ApController,
    action    : "all",
  },
  {
    method    : "get",
    route     : "/aps/:id",
    controller: ApController,
    action    : "one",
  },
  {
    method    : "get",
    route     : "/aps/q/:query",
    controller: ApController,
    action    : "search",
  },
  
];

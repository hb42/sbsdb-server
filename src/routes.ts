import { SwController } from "./controller/spielwieseController";
import { ApController } from "./model_v4/controller/apController";

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

  {
    method    : "get",
    route     : "/sw",
    controller: SwController,
    action    : "all",
  },
  {
    method    : "get",
    route     : "/sw/:id",
    controller: SwController,
    action    : "one",
  },
  {
    method    : "get",
    route     : "/sw/ins/:txt",
    controller: SwController,
    action    : "save",
  },

];

export enum Type {
  NUMBER = 0,
  STRING = 1,
  BOOLEAN = 2,
  RETURN = 3,
  BREAK = 4,
  CONTINUE = 5,
}

export type Return = {
  value: any;
  type: Type;
};

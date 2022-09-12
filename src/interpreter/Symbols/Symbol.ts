import { Type } from "../Abstract/Return";

export class Symbol {
  public value: any;
  public id: string;
  public type: Type;
  public line: number;
  public column: number;

  constructor(
    value: any,
    id: string,
    type: Type,
    line: number,
    column: number
  ) {
    this.value = value;
    this.id = id;
    this.type = type;
    this.line = line;
    this.column = column;
  }
}

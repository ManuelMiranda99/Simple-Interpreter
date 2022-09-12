import { Expression } from "../Abstract/Expression";
import { Type } from "../Abstract/Return";

export class Literal extends Expression {
  constructor(
    private value: any,
    line: number,
    column: number,
    private type: Type
  ) {
    super(line, column);
  }

  public execute() {
    switch (this.type) {
      case Type.NUMBER:
        return { value: Number(this.value), type: Type.NUMBER };
      case Type.STRING:
        return { value: String(this.value), type: Type.STRING };
      case Type.BOOLEAN:
        return { value: Boolean(this.value), type: Type.BOOLEAN };
      default:
        return { value: this.value, type: this.type };
    }
  }
}

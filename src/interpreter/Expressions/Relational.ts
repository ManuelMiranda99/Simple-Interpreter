import { Expression } from "../Abstract/Expression";
import { Type } from "../Abstract/Return";
import { Environment } from "../Symbols/Environment";

export class Relational extends Expression {
  constructor(
    private left: Expression,
    private right: Expression,
    private operator: string,
    line: number,
    column: number
  ) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const left = this.left.execute(environment);
    const right = this.right.execute(environment);

    switch (this.operator) {
      case ">":
        return { value: left.value > right.value, type: Type.BOOLEAN };
      case "<":
        return { value: left.value < right.value, type: Type.BOOLEAN };
      case ">=":
        return { value: left.value >= right.value, type: Type.BOOLEAN };
      case "<=":
        return { value: left.value <= right.value, type: Type.BOOLEAN };
      case "==":
        return { value: left.value === right.value, type: Type.BOOLEAN };
      case "!=":
        return { value: left.value !== right.value, type: Type.BOOLEAN };
      default:
        throw new Error(`Operator ${this.operator} does not exist`);
    }
  }
}

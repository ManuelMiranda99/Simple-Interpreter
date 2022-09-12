import { Expression } from "../Abstract/Expression";
import { Environment } from "../Symbols/Environment";

export class Variable extends Expression {
  constructor(private id: string, line: number, column: number) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const value = environment.getVariable(this.id);
    if (value == null) {
      throw new Error(`Variable ${this.id} does not exist`);
    }
    return value;
  }
}
